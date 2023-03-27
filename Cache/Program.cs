using System.Net;

List<Dictionary<string,int>> keyValues = new();

using var listener = new HttpListener();


listener.Prefixes.Add("http://localhost:27002/");

listener.Start();