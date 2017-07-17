using System;
using System.Linq;
using System.Text;

namespace test
{
	public static class BaseConvert
	{
		const string base36 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		const string base62 = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
		/// <summary>
		/// Converte a binario
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public static string ToBase2(long number)
		{
			return Convert.ToString(number, 2);
		}
		/// <summary>
		/// Convierte a Octal
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public static string ToBase8(long number)
		{
			return Convert.ToString(number, 8);
		}
		/// <summary>
		/// Converte a hexadecimal
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public static string ToBase16(long number)
		{
			return Convert.ToString(number, 16);
		}
		/// <summary>
		/// Converte a código combinado entre números y letras ASCII con un total de 36 caracteres
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public static string ToBase36(long number)
		{
			return ToBase(base36, number);
		}
		/// <summary>
		/// Converte a código combinado entre números y letras mayúsculas y minúsculas ASCII con un total de 62 caracteres
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public static string ToBase62(long number)
		{
			return ToBase(base62, number);
		}
		/// <summary>
		/// Convierte un número a una base con un conjunto de caracteres <paramref name="custom"/> personalizados
		/// </summary>
		/// <param name="custom">Carácteres irrepetibles</param>
		/// <param name="number"></param>
		/// <returns></returns>
		public static string ToBase(string custom, long number)
		{
			var data = (number / custom.Length) > 0
					 ? ToBase(custom, number / custom.Length)
					 : string.Empty;
			return data + custom[(int)(number % custom.Length)];
		}
		/// <summary>
		/// Convierte a código base 64 con la implementación de <see cref="Convert.ToBase64String"/>
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public static string ToBase64(long number)
		{
			return Convert.ToBase64String(BitConverter.GetBytes(number));
		}
		/// <summary>
		/// Convierte a código base 64 con la implementación de <see cref="Convert.ToBase64String"/>
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string ToBase64(string text)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
		}
		/// <summary>
		/// Converte a decimal un código base 36 generado con <see cref="ToBase36"/>
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		public static long FromBase36(string code)
		{
			return FromBase(base36, code);
		}

		/// <summary>
		/// Converte a decimal un código base 62 generado con <see cref="ToBase62"/>
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		public static long FromBase62(string code)
		{
			return FromBase(base62, code);
		}

		/// <summary>
		/// Converte a decimal un código base 64 generado con <see cref="ToBase64"/>
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		public static long FromBase64(string code)
		{
			return BitConverter.ToInt64(Convert.FromBase64String(code), 0);
		}
		/// <summary>
		/// Convierte a decimal un código generado con <see cref="ToBase"/>
		/// </summary>
		/// <param name="custom"></param>
		/// <param name="code"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		public static long FromBase(string custom, string code)
		{
			return FromBase(custom, code, 0);
		}

		private static long FromBase(string custom, string code, int level)
		{
			return code.Length > 0
				? custom.IndexOf(code.Last()) * (long)Math.Pow(custom.Length, level)
					+ FromBase(custom, code.Remove(code.Length - 1), level + 1)
				: 0;
		}

	}
}
