namespace CodingChallangeWebApi.DataAccess
{
    public class ApiResponse<T>
    {
        public bool StatusSucess { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }

        public ApiResponse()
        {
            StatusSucess = true;
        }
    }
}
