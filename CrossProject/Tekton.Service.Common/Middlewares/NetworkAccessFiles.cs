using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Tekton.Service.Common.Middlewares
{

    public class NetworkAccessFiles
    {

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword,
        int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("kernel32.dll")]
        private static extern Boolean CloseHandle(IntPtr hObject);
        public delegate Object fun_leerarchivo(string? filename);
        public delegate void met_accesoarchivo(string? filename);
        //public T LeerArchivoCompartidoLocal<T>(string fileNameArcCompartido, T obj, fun_leerarchivo _fun_leerarchivo)
        //{
        //    T salida = obj;
        //    return salida = (T)_fun_leerarchivo(fileNameArcCompartido);
        //}
        public void AccesoCompartido(IConfiguration _configuration, string? fileNameArcCompartido, met_accesoarchivo _met_accesoarchivo)
        {
            IntPtr admin_token = new IntPtr();

            try
            {

                if (LogonUser(_configuration["CuentaFileServer:Usuario"] ?? "",
                    _configuration["CuentaFileServer:Dominio"] ?? "",
                    _configuration["CuentaFileServer:Clave"] ?? "", 9, 0, ref admin_token) == true)
                {
                    WindowsIdentity f = new WindowsIdentity(admin_token);

                    Action action = () =>
                    {
                        _met_accesoarchivo(fileNameArcCompartido);
                    };
                    WindowsIdentity.RunImpersonated(
                        f.AccessToken,
                        action
                    );
                }
            }
            catch (Exception se)
            {
                int ret = Marshal.GetLastWin32Error();
                throw se;
            }
        }
        public T LeerArchivoCompartido<T>(IConfiguration _configuration, string? fileNameArcCompartido, T obj, fun_leerarchivo _fun_leerarchivo)
        {
            T salida = obj;
            IntPtr admin_token = new IntPtr();

            try
            {

                if (LogonUser(_configuration["CuentaFileServer:Usuario"] ?? "",
                    _configuration["CuentaFileServer:Dominio"] ?? "",
                    _configuration["CuentaFileServer:Clave"] ?? "", 9, 0, ref admin_token) == true)
                {
                    WindowsIdentity f = new WindowsIdentity(admin_token);

                    Action action = () =>
                    {
                        salida = (T)_fun_leerarchivo(fileNameArcCompartido);
                        //var userName = WindowsIdentity.GetCurrent().Name;
                        //bytes = File.ReadAllBytes(fileNameArcCompartido);
                        //stream.Write(data);
                        //stream.Write(data, 0, data.Length);

                    };
                    WindowsIdentity.RunImpersonated(
                        f.AccessToken,
                        action
                    );
                }
            }
            catch (Exception se)
            {
                int ret = Marshal.GetLastWin32Error();
                throw se;
            }
            return salida;

        }


    }
}
