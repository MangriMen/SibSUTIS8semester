#include <map>
#include <fstream>
#include <iostream>
#include <cmath>

using namespace std;

double count_shennon(int n, double *propabilities)
{
    double result = 0.0;
    for (int i = 0; i < n; i++)
    {
        result += -propabilities[i] * log2(propabilities[i]);
    }
    return result;
}

double count_pairs(int n, int size, string* mas, string file)
{
    double sum = 0.0;
    int counter = 0;
    string pair = "";
    map <string, int> f_pair_prop;
    double* pair_propabilities = new double[n * n];

    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < n; j++)
        {
            pair += mas[i];
            pair += mas[j];
            f_pair_prop[pair] = 0;
            pair = "";
        }
    }

    pair = "";
    for (int i = 0; i < size - 1; i++)
    {
        pair += file[i];
        pair += file[i + 1];
        f_pair_prop[pair]++;
        pair = "";
    }

    pair = "";
    counter = 0;
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < n; j++)
        {
            pair += mas[i];
            pair += mas[j];
            pair_propabilities[counter] = f_pair_prop[pair] / (double)(size - 1);
            sum += pair_propabilities[counter];
            cout << pair_propabilities[counter] << endl;
            counter++;
            pair = "";
        }
    }
    cout << endl << sum << endl;

    return count_shennon(counter, pair_propabilities) / 2;
}

int check_prop(int n, double *propabilities, double random)
{
    double left_side = 0.0;
    double right_side = propabilities[0];

    for (int i = 0; i < n; i++)
    {
        if (random >= left_side && random <= right_side)
        {
            return i;
        }
        left_side += propabilities[i];
        right_side += propabilities[i + 1];
    }
}

int main()
{
    int n = 4;
    int check = 0;
    double sum = 0.0;
    int size = 20240;
    srand(time(NULL));

    string f1_file = "";
    string f2_file = "";

    ofstream f1("f1.txt");
    ofstream f2("f2.txt");

    map <string, int> f1_prop;
    map <string, int> f2_prop;

    string mas[] = { "a", "b", "c", "d" };
    double f1_propabilities[] = { 0.25, 0.25, 0.25, 0.25 };
    double f2_propabilities[] = { 0.2, 0.4, 0.3, 0.1 };

    double* real_f1_propabilities = new double[n];
    double* real_f2_propabilities = new double[n];

    for (int i = 0; i < n; i++)
    {
        f1_prop[mas[i]] = 0;
        f2_prop[mas[i]] = 0;
    }

    if (f1.is_open() && f2.is_open())
    {
        for (int i = 0; i < size; i++)
        {
            check = check_prop(n, f1_propabilities, (double)(rand()) / RAND_MAX);
            f1 << mas[check];
            f1_file += mas[check];
            f1_prop[mas[check]]++;

            check = check_prop(n, f2_propabilities, (double)(rand()) / RAND_MAX);
            f2 << mas[check];
            f2_file += mas[check];
            f2_prop[mas[check]]++;
        }
    }
    f1.close();
    f2.close();

    for (int i = 0; i < n; i++)
    {
        real_f1_propabilities[i] = f1_prop[mas[i]] / (double)size;
        real_f2_propabilities[i] = f2_prop[mas[i]] / (double)size;
    }

    cout << "SINGLE SYMBOLS:" << endl;
    for (int i = 0; i < n; i++)
    {
        cout << real_f1_propabilities[i] << " ";
        sum += real_f1_propabilities[i];
    }
    cout << endl << sum << endl;
    sum = 0.0;
    cout << "FILE1: " << count_shennon(n, real_f1_propabilities) << endl;
    for (int i = 0; i < n; i++)
    {
        cout << real_f2_propabilities[i] << " ";
        sum += real_f2_propabilities[i];
    }
    cout << endl << sum << endl << endl;
    sum = 0.0;
    cout << "FILE2: " << count_shennon(n, real_f2_propabilities) << endl;

    cout << "PAIR OF SYMBOLS:" << endl;
    double t1 = count_pairs(n, size, mas, f1_file), t2 = count_pairs(n, size, mas, f2_file);
    cout << "FILE1: " << t1 << endl;
    cout << "FILE2: " << t2 << endl;
}