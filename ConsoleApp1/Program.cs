// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

await using FileStream htmlFile = File.OpenRead("print-demo.html");
await using FileStream imageFile = File.OpenRead("cdc.webp");

using var client = new HttpClient();
client.BaseAddress = new Uri("http://localhost:3333");

using var content = new MultipartFormDataContent();
content.Add(new StreamContent(htmlFile), "files", "index.html");
content.Add(new StreamContent(imageFile), "files", "cdc.webp");

using HttpResponseMessage message = await client.PostAsync("/forms/chromium/convert/html", content);

await using Stream stream = await message.Content.ReadAsStreamAsync();
await using var destination = new FileStream("foo.pdf", FileMode.Create);
await stream.CopyToAsync(destination);
await destination.FlushAsync();
destination.Close();