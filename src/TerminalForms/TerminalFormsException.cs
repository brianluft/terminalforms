namespace TerminalForms;

public class TerminalFormsException(string message) : Exception(message) { }

public class TerminalFormsNativeInteropException(string message)
    : TerminalFormsException(message) { }
