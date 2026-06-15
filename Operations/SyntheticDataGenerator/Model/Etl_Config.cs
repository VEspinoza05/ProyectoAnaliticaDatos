using System.ComponentModel.DataAnnotations.Schema;

namespace Operations.SyntheticDataGenerator.Model;

[Table("etl_config")]
public class EtlConfig
{
    public int Id { get; set; }
    public DateTime BeginDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? Update_At { get; set; }
}