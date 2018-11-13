// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
  production: false,
  // apiUrl : 'http://localhost:50072/api/',
  apiUrl : 'http://13.228.122.102/DITService/api/',
  toastTimeout: 10000,
  reportBaseUrl: 'http://13.228.122.102/Reports/report/',
  reportServerUrl: 'http://13.228.122.102/AramarkReportServer/Pages/ReportViewer.aspx?/'
};
