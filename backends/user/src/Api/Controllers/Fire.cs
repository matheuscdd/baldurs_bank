// using FirebaseAdmin.Auth;

// namespace Api.Controllers;

// public async Task<FirebaseToken?> VerifyFirebaseToken(string idToken)
// {
//     try
//     {
//         var decoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
//         return decoded;
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine(ex.Message);
//         return null;
//     }
// }