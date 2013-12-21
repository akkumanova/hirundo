var hirundo = db.getSiblingDB('hirundo');
hirundo.dropDatabase();

// passowrds: alabala
hirundo = db.getSiblingDB('hirundo');
var users = [
  {
    "Username": "albsi", 
    "Email": "akumanova@yahoo.com", 
    "PasswordHash": "ABMzw8ytOgYmHvyGc4xkvMP/IzUSA5GvKqLBTeyjX06e56J9wauQaQiv5Rdhmc3rTQ==",
    "PasswordSalt": "chyK78xfQ1mOZtWFNN/Oug==",
    "RegistrationDate": new Date("2013-07-09"),
    "Follows": [ "hrisito", "user3" ]
  },
  {
    "Username": "hrisito", 
    "Email": "hristina.sim@gmail.com",
    "PasswordHash": "ABMzw8ytOgYmHvyGc4xkvMP/IzUSA5GvKqLBTeyjX06e56J9wauQaQiv5Rdhmc3rTQ==",
    "PasswordSalt": "chyK78xfQ1mOZtWFNN/Oug==",
    "RegistrationDate": new Date("2013-07-10"),
    "Follows": [ "user3" ]
  },
  {
    "Username": "user3", 
    "Email": "kosta@yahoo.com", 
    "PasswordHash": "ABMzw8ytOgYmHvyGc4xkvMP/IzUSA5GvKqLBTeyjX06e56J9wauQaQiv5Rdhmc3rTQ==",
    "PasswordSalt": "chyK78xfQ1mOZtWFNN/Oug==",
    "RegistrationDate": new Date("2013-10-09"),
    "Follows": [ "albsi", "hrisito" ]
  }
]
hirundo.user.insert(users)

var admin = db.getSiblingDB('admin');
admin.shutdownServer() ;