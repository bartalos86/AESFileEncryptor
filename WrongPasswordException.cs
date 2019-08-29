using System;

[Serializable]
public class WrongPasswordException : Exception{

public WrongPasswordException() : base("The password is wrong")
{  
}

} 