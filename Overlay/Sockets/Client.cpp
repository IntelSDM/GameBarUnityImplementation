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
		std::string message = Client::ReceiveText(); // halts everything here, goes to recieve a message
		if (message.size() == 0)
			return;
		message = message + "\n";
		std::wstring wideString(message.begin(), message.end());
		OutputDebugString(wideString.c_str());
	

		json jsoned = json::parse(message);
		
		json first = jsoned[0];
		if (first["Type"] == "Loot")
		{
			std::lock_guard<std::mutex> lock(ItemMutex);
			{
				ItemList = jsoned;
			}
		}
	}
}
void Client::DrawingHandler()
{
	
		std::lock_guard<std::mutex> lock(ItemMutex);
		{ // locked region
			SetDrawingSession();
			for (json jsonobject : ItemList)
			{
				std::string type = jsonobject["Type"];
		
				
				if (type == "Loot")
				{
					
					LootJson lootjson;
					lootjson.FromJson(jsonobject);
					std::wstring_convert<std::codecvt_utf8_utf16<wchar_t>> converter;
					int x = lootjson.X;
					int y = lootjson.Y;
					std::wstring name = converter.from_bytes(lootjson.Name);
					DrawTextOnSpriteBatch(x, y, name, "Verdana", 11, Colour(255, 0, 0, 255), Centre);
				//SwapChain->FillRectangle(x, y, width, height, Colors::Red);
				}
			}
			PackSpriteSession();

		
		}
	
	

}
std::string Client::ReceiveText()
{
	ByteArray	receivedbytes;
	uint8_t		recvbuffer[BufferSize];

	while (true)
	{
		int32_t Received = recv(Client::Socket, (char*)recvbuffer, BufferSize, 0);

		if (Received < 0)
			break;

		for (int n = 0; n < Received; ++n)
		{
			receivedbytes.push_back(recvbuffer[n]);
		}
		auto breaker = std::find(receivedbytes.begin(), receivedbytes.end(), '|');

		if (breaker != receivedbytes.end())
		{
			// Found the delimiter, construct the string up to the delimiter
			std::string str(receivedbytes.begin(), breaker);
			return str;
		}
		if (Received <= BufferSize)
			break;
		
	}
	std::string str(receivedbytes.begin(), receivedbytes.end());
	return str;
}