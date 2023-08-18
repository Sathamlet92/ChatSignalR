using System.Text.Json.Serialization;

namespace BlazingChat.Shared.Models.Reponse;

public class ResponseOut<T> where T : class
{
    public ResponseOut()
    {
        
    }
    public bool Success { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public T? Data { get; set; }
    public string? Message { get; set; }

    public static ResponseOut<T> CreateResponse(bool success, string message, T? data = null)
    {
        return new ResponseOut<T>{Success = success, Message = message, Data = data};
    }
}
