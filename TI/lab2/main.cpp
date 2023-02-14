#include <map>
#include <string>
#include <fstream>
#include <iostream>
#include <cmath>

using namespace std;

double count_shennon(int n, double* propabilities)
{
    double result = 0.0;
    for (int i = 0; i < n; i++)
    {
        if (propabilities[i] == 0.0)
        {
            continue;
        }
        result += -propabilities[i] * log2(propabilities[i]);
    }
    return result;
}

double count_pairs(int n, int size, string file)
{
    char symbol1;
    char symbol2;
    double sum = 0.0;
    int counter = 0;
    string pair = "";
    map <string, int> f_pair_prop;
    double* pair_propabilities = new double[(n + 1) * (n + 1)];

    for (int i = 0; i < n + 1; i++)
    {
        if (i == 32)
        {
            symbol1 = static_cast<char> (32);
        }
        else
        {
            symbol1 = static_cast<char> (i - 32);
        }
        for (int j = 0; j < n + 1; j++)
        {
            if (j == 32)
            {
                symbol2 = static_cast<char> (32);
            }
            else
            {
                symbol2 = static_cast<char> (j - 32);
            }
            pair += symbol1;
            pair += symbol2;
            f_pair_prop[pair] = 0;
            pair = "";
        }
    }

    pair = "";
    int number;
    int number1;
    int count_pairs = 0;
    int missed_pairs = 0;
    for (int i = 0; i < size - 1; i++)
    {
        number = static_cast<int>(file[i]);
        number1 = static_cast<int>(file[i + 1]);
        if ((number > 0 && number != 32) || (number1 > 0 && number1 != 32))
        {
            missed_pairs++;
            continue;
        }
        if (number <= -33)
        {
            number += 32;
        }
        if (number1 <= -33)
        {
            number1 += 32;
        }
        pair += static_cast<char> (number);
        pair += static_cast<char> (number1);
        f_pair_prop[pair]++;
        count_pairs++;
        pair = "";
    }
    //cout << count_pairs << endl;

    pair = "";
    counter = 0;
    for (int i = 0; i < n + 1; i++)
    {
        if (i == 32)
        {
            symbol1 = static_cast<char> (32);
        }
        else
        {
            symbol1 = static_cast<char> (i - 32);
        }
        for (int j = 0; j < n + 1; j++)
        {
            if (j == 32)
            {
                symbol2 = static_cast<char> (32);
            }
            else
            {
                symbol2 = static_cast<char> (j - 32);
            }
            pair += symbol1;
            pair += symbol2;
            pair_propabilities[counter] = f_pair_prop[pair] / (double)(count_pairs);
            sum += pair_propabilities[counter];
            counter++;
            pair = "";
        }
    }
    return count_shennon(counter, pair_propabilities) / 2;
}

int main()
{
    srand(time(NULL));
    setlocale(LC_ALL, "Rus");

    int n = 32;
    string line;
    map<int, int> file_prop;

    string tail_file = "";
    ifstream f1("text2.txt");
    double* propabilities = new double[n];

    if (f1.is_open())
    {
        while (getline(f1, line))
        {
            tail_file += line;
        }
    }
    f1.close();

    for (int i = 0; i < n; i++)
    {
        file_prop[i] = 0;
    }

    int number = 0;
    for (int i = 0; i < tail_file.size(); i++)
    {
        number = static_cast<int> (tail_file[i]);
        if (number == n)
        {
            file_prop[number - 1]++;
        }
        else if (number == -72 || number == -88)
        {
            file_prop[5]++;
        }
        else if (number == -4)
        {
            file_prop[26]++;
        }
        else
        {
            number += n;
            if (number < 0)
            {
                number += n;
            }
            if (number >= 29 && number <= 31)
            {
                file_prop[number - 1]++;
            }
            else
            {
                file_prop[number]++;
            }
        }
    }

    char symbol;
    int sum = 0;
    double prop_sum = 0.0;
    for (int i = 0; i < n; i++)
    {
        sum += file_prop[i];
        if (i >= 29 && i <= 31)
        {
            symbol = static_cast<char> (i - 31);
        }
        else
        {
            symbol = static_cast<char> (i - 32);
        }
    }

    for (int i = 0; i < n; i++)
    {
        propabilities[i] = file_prop[i] / (double)sum;
        prop_sum += propabilities[i];
        if (i >= 29 && i <= 31)
        {
            symbol = static_cast<char> (i - 31);
        }
        else
        {
            symbol = static_cast<char> (i - 32);
        }
    }
    cout << prop_sum << endl;

    cout << "Entropy for single characters: " << count_shennon(n, propabilities) << endl;
    cout << "Entropy for pairs: " <<count_pairs(n, sum, tail_file) << endl;
}