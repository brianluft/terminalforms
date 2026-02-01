using TerminalForms;

namespace TerminalFormsDemo.Calculator;

/// <summary>
/// Interactive calculator demo showcasing Form, TextBox, Button, and Label controls.
/// Demonstrates event handling, user input validation, and multi-control interaction.
/// Uses instance fields and methods - safe because Application.OpenForms keeps the form alive.
/// </summary>
public class CalculatorDemo : IDemo
{
    // Controls
    private Form _form = null!;
    private TextBox _display = null!;
    private Label _errorLabel = null!;

    // Calculator state
    private decimal _accumulator;
    private char _pendingOperation;
    private bool _startNewNumber = true;

    public void Setup()
    {
        // Form sized to fit calculator layout
        _form = new Form { Text = "Calc", Bounds = new Rectangle(0, 0, 26, 11) };

        // Display textbox at top
        _display = new TextBox { Bounds = new Rectangle(1, 1, 22, 1), Text = "0" };

        // Error label at bottom
        _errorLabel = new Label
        {
            Bounds = new Rectangle(1, 10, 22, 1),
            Text = "",
            UseMnemonic = false,
        };

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

        // Wire up event handlers using instance methods
        btn0.Click += (_, _) => OnDigitClick("0");
        btn1.Click += (_, _) => OnDigitClick("1");
        btn2.Click += (_, _) => OnDigitClick("2");
        btn3.Click += (_, _) => OnDigitClick("3");
        btn4.Click += (_, _) => OnDigitClick("4");
        btn5.Click += (_, _) => OnDigitClick("5");
        btn6.Click += (_, _) => OnDigitClick("6");
        btn7.Click += (_, _) => OnDigitClick("7");
        btn8.Click += (_, _) => OnDigitClick("8");
        btn9.Click += (_, _) => OnDigitClick("9");

        btnAdd.Click += (_, _) => OnOperatorClick('+');
        btnSub.Click += (_, _) => OnOperatorClick('-');
        btnMul.Click += (_, _) => OnOperatorClick('*');
        btnDiv.Click += (_, _) => OnOperatorClick('/');

        btnEq.Click += OnEqualsClick;
        btnClr.Click += OnClearClick;

        // Add controls to form
        _form.Controls.Add(_display);
        _form.Controls.Add(btn7);
        _form.Controls.Add(btn8);
        _form.Controls.Add(btn9);
        _form.Controls.Add(btnAdd);
        _form.Controls.Add(btn4);
        _form.Controls.Add(btn5);
        _form.Controls.Add(btn6);
        _form.Controls.Add(btnSub);
        _form.Controls.Add(btn1);
        _form.Controls.Add(btn2);
        _form.Controls.Add(btn3);
        _form.Controls.Add(btnMul);
        _form.Controls.Add(btn0);
        _form.Controls.Add(btnEq);
        _form.Controls.Add(btnClr);
        _form.Controls.Add(btnDiv);
        _form.Controls.Add(_errorLabel);

        _form.Show();
    }

    private void OnDigitClick(string digit)
    {
        _errorLabel.Text = "";
        if (_startNewNumber)
        {
            _display.Text = digit;
            _startNewNumber = false;
        }
        else
        {
            _display.Text += digit;
        }
    }

    private void OnOperatorClick(char op)
    {
        _errorLabel.Text = "";
        if (decimal.TryParse(_display.Text, out var value))
        {
            _accumulator = value;
            _pendingOperation = op;
            _startNewNumber = true;
        }
    }

    private void OnEqualsClick(object? sender, EventArgs e)
    {
        _errorLabel.Text = "";
        if (_pendingOperation != '\0' && decimal.TryParse(_display.Text, out var operand))
        {
            try
            {
                _accumulator = _pendingOperation switch
                {
                    '+' => _accumulator + operand,
                    '-' => _accumulator - operand,
                    '*' => _accumulator * operand,
                    '/' => operand == 0
                        ? throw new DivideByZeroException()
                        : _accumulator / operand,
                    _ => _accumulator,
                };
                _display.Text = _accumulator.ToString("G");
            }
            catch (DivideByZeroException)
            {
                _errorLabel.Text = "Divide by zero";
                _display.Text = "0";
                _accumulator = 0;
            }
            _pendingOperation = '\0';
            _startNewNumber = true;
        }
    }

    private void OnClearClick(object? sender, EventArgs e)
    {
        _accumulator = 0;
        _pendingOperation = '\0';
        _startNewNumber = true;
        _display.Text = "0";
        _errorLabel.Text = "";
    }
}
