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
5. Go to `bower_components` folder
  * download image-upload running `git clone https://github.com/Mischi/angularjs-imageupload-directive.git`
6. Open the solution `Hirundo.sln` and run the web project `Hirundo.Web`

#### Create database
First install Mongo as described here: http://docs.mongodb.org/manual/tutorial/install-mongodb-on-windows/
In Hirundo.Database folder install lodash and mongodb with the following commands:
  * `npm install mongodb` 
  * `npm install lodash`
Then in the Hirundo.Database run CreateDatabase.tt tool.