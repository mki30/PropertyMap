using System;

/// <summary>
/// Summary description for Messages
/// </summary>
public class Messages
{
    public Messages()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static Exception LastException
    {
        get
        {
            return _LastException;
        }
        set
        {
            if (value != _LastException)
            {
                _LastException = value;
            }
        }
    }

    private static Exception _LastException;

}