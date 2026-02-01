using TerminalForms;

namespace TerminalFormsDemo.Calculator;

/// <summary>
/// Interactive calculator demo showcasing Form, TextBox, Button, and Label controls.
/// Demonstrates event handling, user input validation, and multi-control interaction.
/// </summary>
public class CalculatorDemo : IDemo
{
    public void Setup()
    {
        // Calculator state - local variables captured by lambdas
        // IMPORTANT: Track displayText ourselves - reading from control.Text in click handlers crashes!
        string displayText = "0";
        decimal accumulator = 0;
        char pendingOperation = '\0';
        bool startNewNumber = true;

        // Form sized to fit calculator layout
        var form = new Form { Text = "Calc", Bounds = new Rectangle(0, 0, 26, 11) };

        // Display textbox at top
        var display = new TextBox { Bounds = new Rectangle(1, 1, 22, 1), Text = "0" };

        // Error label at bottom
        var errorLabel = new Label
        {
            Bounds = new Rectangle(1, 10, 22, 1),
            Text = "",
            UseMnemonic = false,
        };

        // Helper to update display
        void SetDisplay(string text)
        {
            displayText = text;
            display.Text = text;
        }

        // Buttons: 5 wide, 2 tall each
        // Row 1 (y=2): 7, 8, 9, +
        var btn7 = new Button { Bounds = new Rectangle(1, 2, 5, 2), Text = "7" };
        var btn8 = new Button { Bounds = new Rectangle(7, 2, 5, 2), Text = "8" };
        var btn9 = new Button { Bounds = new Rectangle(13, 2, 5, 2), Text = "9" };
        var btnAdd = new Button { Bounds = new Rectangle(19, 2, 5, 2), Text = "+" };

        // Row 2 (y=4): 4, 5, 6, -
        var btn4 = new Button { Bounds = new Rectangle(1, 4, 5, 2), Text = "4" };
        var btn5 = new Button { Bounds = new Rectangle(7, 4, 5, 2), Text = "5" };
        var btn6 = new Button { Bounds = new Rectangle(13, 4, 5, 2), Text = "6" };
        var btnSub = new Button { Bounds = new Rectangle(19, 4, 5, 2), Text = "-" };

        // Row 3 (y=6): 1, 2, 3, *
        var btn1 = new Button { Bounds = new Rectangle(1, 6, 5, 2), Text = "1" };
        var btn2 = new Button { Bounds = new Rectangle(7, 6, 5, 2), Text = "2" };
        var btn3 = new Button { Bounds = new Rectangle(13, 6, 5, 2), Text = "3" };
        var btnMul = new Button { Bounds = new Rectangle(19, 6, 5, 2), Text = "*" };

        // Row 4 (y=8): 0, =, C, /
        var btn0 = new Button { Bounds = new Rectangle(1, 8, 5, 2), Text = "0" };
        var btnEq = new Button { Bounds = new Rectangle(7, 8, 5, 2), Text = "=" };
        var btnClr = new Button { Bounds = new Rectangle(13, 8, 5, 2), Text = "~C~" };
        var btnDiv = new Button { Bounds = new Rectangle(19, 8, 5, 2), Text = "/" };

        // Digit button click handlers - use local displayText, not display.Text
        btn0.Click += (sender, e) =>
        {
            errorLabel.Text = "";
            if (startNewNumber)
            {
                SetDisplay("0");
                startNewNumber = false;
            }
            else
            {
                SetDisplay(displayText + "0");
            }
        };
        btn1.Click += (sender, e) =>
        {
            errorLabel.Text = "";
            if (startNewNumber)
            {
                SetDisplay("1");
                startNewNumber = false;
            }
            else
            {
                SetDisplay(displayText + "1");
            }
        };
        btn2.Click += (sender, e) =>
        {
            errorLabel.Text = "";
            if (startNewNumber)
            {
                SetDisplay("2");
                startNewNumber = false;
            }
            else
            {
                SetDisplay(displayText + "2");
            }
        };
        btn3.Click += (sender, e) =>
        {
            errorLabel.Text = "";
            if (startNewNumber)
            {
                SetDisplay("3");
                startNewNumber = false;
            }
            else
            {
                SetDisplay(displayText + "3");
            }
        };
        btn4.Click += (sender, e) =>
        {
            errorLabel.Text = "";
            if (startNewNumber)
            {
                SetDisplay("4");
                startNewNumber = false;
            }
            else
            {
                SetDisplay(displayText + "4");
            }
        };
        btn5.Click += (sender, e) =>
        {
            errorLabel.Text = "";
            if (startNewNumber)
            {
                SetDisplay("5");
                startNewNumber = false;
            }
            else
            {
                SetDisplay(displayText + "5");
            }
        };
        btn6.Click += (sender, e) =>
        {
            errorLabel.Text = "";
            if (startNewNumber)
            {
                SetDisplay("6");
                startNewNumber = false;
            }
            else
            {
                SetDisplay(displayText + "6");
            }
        };
        btn7.Click += (sender, e) =>
        {
            errorLabel.Text = "";
            if (startNewNumber)
            {
                SetDisplay("7");
                startNewNumber = false;
            }
            else
            {
                SetDisplay(displayText + "7");
            }
        };
        btn8.Click += (sender, e) =>
        {
            errorLabel.Text = "";
            if (startNewNumber)
            {
                SetDisplay("8");
                startNewNumber = false;
            }
            else
            {
                SetDisplay(displayText + "8");
            }
        };
        btn9.Click += (sender, e) =>
        {
            errorLabel.Text = "";
            if (startNewNumber)
            {
                SetDisplay("9");
                startNewNumber = false;
            }
            else
            {
                SetDisplay(displayText + "9");
            }
        };

        // Operator button click handlers - use displayText for parsing
        btnAdd.Click += (sender, e) =>
        {
            errorLabel.Text = "";
            if (decimal.TryParse(displayText, out var value))
            {
                accumulator = value;
                pendingOperation = '+';
                startNewNumber = true;
            }
        };
        btnSub.Click += (sender, e) =>
        {
            errorLabel.Text = "";
            if (decimal.TryParse(displayText, out var value))
            {
                accumulator = value;
                pendingOperation = '-';
                startNewNumber = true;
            }
        };
        btnMul.Click += (sender, e) =>
        {
            errorLabel.Text = "";
            if (decimal.TryParse(displayText, out var value))
            {
                accumulator = value;
                pendingOperation = '*';
                startNewNumber = true;
            }
        };
        btnDiv.Click += (sender, e) =>
        {
            errorLabel.Text = "";
            if (decimal.TryParse(displayText, out var value))
            {
                accumulator = value;
                pendingOperation = '/';
                startNewNumber = true;
            }
        };

        // Equals button - use displayText for parsing
        btnEq.Click += (sender, e) =>
        {
            errorLabel.Text = "";
            if (pendingOperation != '\0' && decimal.TryParse(displayText, out var operand))
            {
                try
                {
                    accumulator = pendingOperation switch
                    {
                        '+' => accumulator + operand,
                        '-' => accumulator - operand,
                        '*' => accumulator * operand,
                        '/' => operand == 0
                            ? throw new DivideByZeroException()
                            : accumulator / operand,
                        _ => accumulator,
                    };
                    SetDisplay(accumulator.ToString("G"));
                }
                catch (DivideByZeroException)
                {
                    errorLabel.Text = "Divide by zero";
                    SetDisplay("0");
                    accumulator = 0;
                }
                pendingOperation = '\0';
                startNewNumber = true;
            }
        };

        // Clear button
        btnClr.Click += (sender, e) =>
        {
            accumulator = 0;
            pendingOperation = '\0';
            startNewNumber = true;
            SetDisplay("0");
            errorLabel.Text = "";
        };

        // Add controls to form
        form.Controls.Add(display);
        form.Controls.Add(btn7);
        form.Controls.Add(btn8);
        form.Controls.Add(btn9);
        form.Controls.Add(btnAdd);
        form.Controls.Add(btn4);
        form.Controls.Add(btn5);
        form.Controls.Add(btn6);
        form.Controls.Add(btnSub);
        form.Controls.Add(btn1);
        form.Controls.Add(btn2);
        form.Controls.Add(btn3);
        form.Controls.Add(btnMul);
        form.Controls.Add(btn0);
        form.Controls.Add(btnEq);
        form.Controls.Add(btnClr);
        form.Controls.Add(btnDiv);
        form.Controls.Add(errorLabel);

        form.Show();
    }
}
