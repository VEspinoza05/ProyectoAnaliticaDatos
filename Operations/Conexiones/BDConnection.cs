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
			BDOrigen = SqlADOConexion.BuildDataMapper(".", "sa", "12345678", "BDOrigen");
			BDDestino = SqlADOConexion.BuildDataMapper(".", "sa", "12345678", "BDDestino");
			BDDestino?.GDatos.TestConnection();
			BDOrigen?.GDatos.TestConnection();
		}

		public bool InitMainConnection(bool isDebug = false)
		{
			return SqlADOConexion.IniciarConexion("sa", "12345678", "localhost", "DW_Bienestar_Psicoemocional");
		}
	}
}