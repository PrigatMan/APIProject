namespace APIProject.Model
{
    public class Result
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = "Something went wrong";
        public List<string> Errors { get; set; } = new List<string>();
    }
}
