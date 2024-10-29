namespace Core.DTOs
{
    public record EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
    }
}
