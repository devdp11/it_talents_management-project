namespace Frontend.Models;

public class CreateWorkProposal
{
    public int UserID { get; set; }
    public string Name { get; set; }
    public int ClientID { get; set; }
    public string TalentCategory { get; set; }
    public int TotalHours { get; set; }
    public string JobDescription { get; set; }
}