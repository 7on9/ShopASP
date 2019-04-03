// #include <iostream>
 
// using namespace std;

// int n;
// int m;

  
// int main() {
    
//     cin >> n;
//     int A[n+1][n+1];
//     m = n*n;
//     int v = 0;
//     while(m > 0){
//         for(int i = n - v; i > v; i--){
//             if(m == 0)
//                 break;
//             A[i][v+1] = m--;

//         }
//         for(int i = v+2; i <= n-v; i++){
//             if(m == 0)
//                 break;
//          A[v+1][i] = m--;

//         }
//         for(int i = v+2; i <= n-v; i++){
//             if(m == 0)
//                 break;
//             A[i][n-v] = m--;
//         }
//         for(int i = n-v-1; i >= v+2; i--){
//             if(m == 0)
//                 break;
//             A[n-v][i] = m--;

//         }
//         v++;
//     }


//     for(int i = 1; i <= n; i++){
//         for(int j = 1; j <= n; j++){
//             cout << A[i][j]  << " ";
//         }
//         cout << "\n";
//     }
        
        
// }

#include <iostream>
#include <String>
 
using namespace std;
int main() {
    string s, res = "";
    cin.getline(s, stdin);
    bool isWord = false;
    if()
    for(int i = 0; i < s.size(); i++){
        if(s[i] == ' '){
            if(isWord)
                res += ' ';
            isWord = false;
        } else {
            isWord = true;
            res += s[i];
        }
    }
    if(res[res.size()] == ' ')
        res--;
    cout << res << endl;
    return 0;
}