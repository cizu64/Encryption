// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Encryption;


string key = "251EFF08456499042EC695C0B20D4FE2BF0F1B03D71608E7CF1CF1DCFE99D212";

var body = new
{
	amount = 200,
	cardexpirydate = "2605",
	cvv = "707",
	datetime = "240218003431",
	merchantid = "2058LA000000001",
	merchantname = "BelemaPay Limited LA LANG",
	otp = "633178",
	pan = "5399839226202454",
	terminalid = "20580001",
	transactionid = "457864436867"
};

string serialized = JsonSerializer.Serialize(body);

Console.WriteLine("----------------------------------------------");

Console.WriteLine("Serialized data" + serialized);

Console.WriteLine("----------------------------------------------\n");


string result = AESCryptography.Encrypt(serialized, key);

Console.WriteLine("----------------------------------------------");

Console.WriteLine($"Encrypted result\n{result}");
Console.WriteLine("----------------------------------------------\n");


Console.WriteLine("----------------------------------------------");

Console.WriteLine($"Decrypted result");

string decrypotedResult = AESCryptography.Decrypt(result, key);

Console.WriteLine(decrypotedResult);

Console.WriteLine("----------------------------------------------\n");

