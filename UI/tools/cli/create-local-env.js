var fs = require("fs");

const TestURL = "https://localhost:5001/api/v1";

const DeployURL = "https://unibuy.azurewebsites.net/api/v1";

function CreateLocalEnvFile(tenant) {
  url = DeployURL;
  if (tenant == "test") {
    url = TestURL;
  }

  content = `REACT_APP_URL=${url}`;

  fs.writeFile(".env.local", content, function (err) {
    if (err) throw err;
    console.log(".env.local was updated");
  });
}

CreateLocalEnvFile(process.argv[2]);
