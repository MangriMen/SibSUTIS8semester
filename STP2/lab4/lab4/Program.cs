using lab4;

try
{
    Matrix a = new(3, 3);

    Matrix b = new(3, 3);

    Matrix c;
    for (int i = 0; i < a.Rows; i++)
    {
        for (int j = 0; j < a.Cols; j++)
        {
            a[i, j] = a.Cols * i + j;
        }
    }

    a.Show();
    for (int i = 0; i < a.Rows; i++)
    {
        for (int j = 0; j < a.Cols; j++)
        {
            b[i, j] = a.Cols * i + j + 1;
        }
    }

    b.Show();

    c = a + b;

    c.Show();

    Console.WriteLine(c);
}
catch (MyException e)
{
    Console.WriteLine(e.Message);
}
