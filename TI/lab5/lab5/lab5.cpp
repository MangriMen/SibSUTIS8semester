#include <iostream>
#include <fstream>
#include <string>
#include <time.h>
#include <cmath>
#include <vector>
#include <tuple>
#include <algorithm>
#include <set>

using namespace std;

string to_bin(int num, int n) {
    string result(n, '0');

    for (size_t i = 0; i < n; i++) {
        result[i] = (char)(num % 2) + 48;
        num /= 2;
    }

    return result;
}


int to_int(string s) {
    int result = 0;
    int num = 0;

    for (int i = static_cast<long long>(s.length()) - 1; i >= 0; i--) {
        if (s[i] == '1') {
            result += static_cast<int>(pow(2, num));
        }
        num++;
    }
    
    return result;
}

vector<string> CreateSamplingMatrix(int n) {
    vector<string> sampling_matrix(static_cast<int>(pow(2, n)));

    for (int i = 0; i < sampling_matrix.size(); i++) {
        sampling_matrix[i] = to_bin(i, n);
        reverse(sampling_matrix[i].begin(), sampling_matrix[i].end());
    }

    return sampling_matrix;
}

vector<string> GenerateCodeWords(const vector<string>& generating_matrix, int n, int m, const vector<string>& sampling_matrix) {
    vector<string> code_words(static_cast<int>(pow(2, n)), string(m, '0'));

    for (int i = 0; i < code_words.size(); i++) {
        for (int j = 0; j < n; j++) {
            if (sampling_matrix[i][j] == '0') continue;

            for (int t = 0; t < m; t++) {
                if (
                    (generating_matrix[j][t] == '0' && code_words[i][t] == '1') ||
                    (generating_matrix[j][t] == '1' && code_words[i][t] == '0')
                    ) {
                    code_words[i][t] = '1';
                }
                else {
                    code_words[i][t] = '0';
                }
            }
        }
    }

    return code_words;
}

int dmin(vector<string> code_words) {
    int dmin = INT_MAX;

    for (int i = 0; i < code_words.size() - 1; i++) {
        for (int j = i + 1; j < code_words.size(); j++) {
            int dif = 0;
            for (int k = 0; k < code_words[j].size(); k++)
            {
                if (code_words[i][k] != code_words[j][k])
                    dif++;
            }
            if (dif < dmin && dif != 0) {
                dmin = dif;
            }

        }
    }
    return dmin;
}

void randomMat(string fname) {
    ofstream file_out(fname);
    if (!file_out.is_open()) {
        throw new exception("Cannot open matrix file");
    }

    int n = rand() % 8 + 3;
    int m = rand() % 8 + 3;

    file_out << n << " " << m << endl;

    for (int i = 0; i < n; i++) {
        for (int j = 0; j < m; j++) {
            file_out << rand() % 2 << " ";
        }
        file_out << "\n";
    }

    file_out.close();
}

tuple<vector<string>, int, int> ReadMatrix(string filename) {
    ifstream file_in(filename);
    if (!file_in.is_open()) {
        throw new exception("Cannot open matrix file");
    }

    int n = 0;
    int m = 0;

    file_in >> n >> m;

    vector<string> generating_matrix(static_cast<int>(pow(2, n)), string(m, '0'));

    for (int i = 0; i < n; i++) {
        for (int j = 0; j < m; j++) {
            file_in >> generating_matrix[i][j];
        }
    }

    file_in.close();

    return make_tuple(generating_matrix, n, m);
}

int main()
{
    srand(time(0));

    auto [generating_matrix, n, m] = ReadMatrix("input.txt");

    auto sampling_matrix = CreateSamplingMatrix(n);
    auto code_words = GenerateCodeWords(generating_matrix, n, m, sampling_matrix);

    auto d_min = dmin(code_words);

    cout << "Sampling Matrix" << endl;
    for (const auto& sample : sampling_matrix) {
        cout << sample << endl;
    }

    cout << "\nCode words:" << endl;
    for (const auto& word : code_words) {
        cout << word << endl;
    }

    cout << "Code dimension: " << n << endl;
    cout << "Number of code words: " << code_words.size() << endl;
    cout << "Dmin: " << d_min << endl;

    return 0;
}
