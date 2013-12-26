hirundo
=======

#### Build
1. Install [NodeJS](http://nodejs.org/)
2. Install `grunt-cli` globally with `npm install -g grunt-cli`
3. Install `bower` globally with `npm install -g bower`
4. In the js app folder `cd .\Hirundo.Web\App`
  * Install the required npm packages with `npm install`
  * Install the required bower components with `bower install`
  * Build the js app with `grunt`
  * Build the `angular-bootstrap`   
    `cd .\bower_components\angular-bootstrap`  
    `npm install`  
    `grunt before-test`  
    `grunt after-test`  
    `cd ..\..\`
  * Build the js app with `grunt`
5. Open the solution `Hirundo.sln` and run the web project `Hirundo.Web`

#### Create database
First install Mongo as described here: http://docs.mongodb.org/manual/tutorial/install-mongodb-on-windows/
In Hirundo.Database folder install lodash and mongodb with the following commands:
  * `npm install mongodb` 
  * `npm install lodash`
Then in the Hirundo.Database run CreateDatabase.tt tool.