#include "pch.h"
#include "Client.h"
#include "Graphics.h"
#include "Drawing.h"
constexpr int BufferSize = 4096;
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
		std::lock_guard<std::mutex> lock(RectangleListMutex);
		{
			JsonList = jsoned;
		}
	/*	std::string type = jsoned["Type"];
		if (type == "Rectangle")
		{
			test = jsoned.dump();
			RectangleJson rectangle(0.0f, 0.0f, 0.0f, 0.0f);
			rectangle.FromJson(jsoned);
			std::lock_guard<std::mutex> lock(RectangleListMutex);
			{
				RectangleList.push_back(rectangle);
			}
		}
		*/
		// Iterate through each JSON object in the array and convert to RectangleJson
	/*	for (const json& js : JsonList) {
			std::string type = js["Type"];
			
			std::wstring wideString(type.begin(), type.end());
			OutputDebugString(wideString.c_str());
		}*/
	}
}
void Client::DrawingHandler()
{
	
		std::lock_guard<std::mutex> lock(RectangleListMutex);
		{ // locked region
			for (json jsonobject : JsonList)
			{

				
				//OutputDebugString(L"Draw");
				//json jsoned = json::parse(jsonobject);
				std::string type = jsonobject["Type"];
				
				RectangleJson rectangle(0.0f, 0.0f, 0.0f, 0.0f);
				if (type == "Rectangle")
				{
					
				
					rectangle.FromJson(jsonobject);
					//std::string xx = std::to_string(rectangle.Width);
					//std::wstring wideString(xx.begin(), xx.end());
					//OutputDebugString(wideString.c_str());
				int x = rectangle.X;
				int y = rectangle.Y;
				int width = rectangle.Width;
				int height = rectangle.Height;

				SwapChain->FillRectangle(x, y, width, height, Colors::Red);
				}
			}

			RectangleList.clear();
		}
	
	std::wstring widetext(test.begin(), test.end());
	Platform::String^ text = ref new Platform::String(widetext.c_str());
	SwapChain->DrawText(text, 100, 100, Colors::Red);

}
std::string Client::ReceiveText()
{
	ByteArray	ReceivedBytes;
	uint8_t		RecvBuffer[BufferSize];

	while (true)
	{
		int32_t Received = recv(Client::Socket, (char*)RecvBuffer, BufferSize, 0);

		if (Received < 0)
			break;

		for (int n = 0; n < Received; ++n)
		{
			ReceivedBytes.push_back(RecvBuffer[n]);
		}
		auto breaker = std::find(ReceivedBytes.begin(), ReceivedBytes.end(), '|');

		if (breaker != ReceivedBytes.end())
		{
			// Found the delimiter, construct the string up to the delimiter
			std::string str(ReceivedBytes.begin(), breaker);
			return str;
		}
		if (Received <= BufferSize)
			break;

	}
	std::string str(ReceivedBytes.begin(), ReceivedBytes.end());
	return str;
}