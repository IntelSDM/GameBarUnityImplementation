#include "pch.h"
#include "Client.h"
#include "Graphics.h"
#include "Drawing.h"
#include "LootJson.h"
#include <locale>
#include <codecvt>
constexpr int BufferSize = 1000000;
void Client::SendText(std::string text)
{
	ByteArray plaintext(text.begin(), text.end());
	int32_t Result = send(Client::Socket, (char*)plaintext.data(), (int)plaintext.size(), 0);
}
void Client::MessageHandler()
{
	bool jump = false;
	
	while (true)
	{
		std::string message = Client::ReceiveText();
		if (message.size() == 0)
			return;

		json jsoned = json::parse(message);
		if (jsoned[0]["Type"] == "Loot")
		{
			std::lock_guard<std::mutex> lock(ItemMutex);
			NewItemBuffer =jsoned;
		}
	}
}
void Client::DrawingHandler()
{
	std::list<json> localitems;
	{
		std::lock_guard<std::mutex> lock(ItemMutex);
		std::swap(ItemList, NewItemBuffer);
	}
	{
		std::lock_guard<std::mutex> lock(ItemMutex);
		localitems = ItemList;
	}


	SetDrawingSession();
	for (json jsonobject : localitems)
	{

		if (jsonobject["Type"] == "Loot")
		{

			LootJson lootjson;
			lootjson.FromJson(jsonobject);
			std::wstring_convert<std::codecvt_utf8_utf16<wchar_t>> converter;
			int x = lootjson.X;
			int y = lootjson.Y;
			std::wstring name = converter.from_bytes(lootjson.Name);
			DrawTextOnSpriteBatch(x, y, name, "Verdana", 11, Colour(255, 0, 0, 255), Centre);
		}
	}
	PackSpriteSession();
}
std::string Client::ReceiveText()
{
	ByteArray	receivedbytes;
	uint8_t		recvbuffer[BufferSize];

	while (true)
	{
		int32_t received = recv(Client::Socket, (char*)recvbuffer, BufferSize, 0);

		if (received < 0)
			break;

		for (int n = 0; n < received; ++n)
		{
			receivedbytes.push_back(recvbuffer[n]);
		}
		auto breaker = std::find(receivedbytes.begin(), receivedbytes.end(), '|');

		if (breaker != receivedbytes.end())
		{
			std::string str(receivedbytes.begin(), breaker);
			return str;
		}
		if (received <= BufferSize)
			break;
		
	}
	std::string str(receivedbytes.begin(), receivedbytes.end());
	return str;
}