hirundo = db.getSiblingDB('hirundo');
hirundo.dropDatabase();

hirundo = db.getSiblingDB('hirundo');
users = [
  {
    "username": "user1", 
    "email": "alabala@yahoo.com", 
    "password": "1234",
    "registrationDate": new Date("2013-07-09"),
    "follows": [ "user2", "user3" ],
    "verified": false
  },
  {
    "username": "user2", 
    "email": "niki@yahoo.com", 
    "password": "1234", 
    "registrationDate": new Date("2013-07-10"),
    "follows": [ "user3" ],
    "verified": false
  },
  {
    "username": "user3", 
    "email": "kosta@yahoo.com", 
    "password": "1234", 
    "registrationDate": new Date("2013-10-09"),
    "follows": [ "user1", "user2" ],
  "verified": false
  }
]
hirundo.users.insert(users)

admin = db.getSiblingDB('admin');
admin.shutdownServer() ;