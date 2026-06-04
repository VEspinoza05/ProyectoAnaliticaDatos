using APPCORE;
using APPCORE.BDCore.Abstracts;
namespace BusinessLogic.Connection
{
	public class BDConnection
	{
		public WDataMapper? BDOrigen { get; set; }
		public WDataMapper? BDDestino { get; set; }
		public BDConnection()
		{
			BDOrigen = SqlADOConexion.BuildDataMapper("VLADIMIR_PC", "sa", "12345678", "BDOrigen");
			BDDestino = SqlADOConexion.BuildDataMapper("VLADIMIR_PC", "sa", "12345678", "BDDestino");
			BDDestino?.GDatos.TestConnection();
			BDOrigen?.GDatos.TestConnection();
		}

		public bool InitMainConnection(bool isDebug = false)
		{
			return SqlADOConexion.IniciarConexion("sa", "12345678", "VLADIMIR_PC", "DW_Bienestar_Psicoemocional");
		}
	}
}