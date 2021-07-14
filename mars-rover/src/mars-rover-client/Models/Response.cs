namespace mars_rover_client.Models
{
    public class Response<T>
    {
        public T Data { get; set; }

        public int HttpStatusCode { get; set; }

        public bool IsSuccessStatusCode { get; set; }
    }
}
