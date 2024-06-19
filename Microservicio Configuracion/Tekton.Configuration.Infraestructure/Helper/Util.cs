namespace Tekton.Configuration.Infraestructure.Helper;

public static class Util
{
    public static string BoolToString(this bool? value)
    {
        if (!value.HasValue) return string.Empty;

        return value.Value ? "SI" : "NO";
    }

    public static string GenerarRuta(string ruta)
    {
        return ruta;
    }
}
