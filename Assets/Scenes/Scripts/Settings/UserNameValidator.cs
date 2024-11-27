using System.Text;

public static class UserNameValidator
{
    static public bool CheckName(string name, out string error_message)
    {
        if (string.IsNullOrEmpty(name))
        {
            error_message = "Имя не должно быть пустым";
            return false;
        }
        if (Encoding.Default.GetBytes(name).Length > 20)
        {
            error_message = "Имя не должно быть длинне 20 символов";
            return false;
        }
        error_message = null;
        return true;
    }
}

