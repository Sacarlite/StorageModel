namespace Warehouse.Infrastructure.Service
{
    internal class PathService
    {
        public static string GetCurentFolderPath(string additionPath = "")
        {
            try
            {
                return $"{Environment.CurrentDirectory}\\{additionPath}";
            }
            catch (Exception)
            {
                throw new Exception("Невозможно установить путь к используемой директории");
            }
        }
    }
}
