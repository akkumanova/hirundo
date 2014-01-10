var MongoClient = require('mongodb').MongoClient,
    ObjectID = require('mongodb').ObjectID,
    GridStore = require('mongodb').GridStore,
    _ = require('lodash');

var albsiId = new ObjectID(),
    hrtisitoId = new ObjectID(),
    userId = new ObjectID();

var users = [
  {
    "_id": albsiId, //passowrd: alabala 
    "Username": "albsi",
    "Fullname": "Albena Kumanova",
    "Email": "akumanova@yahoo.com",
    "PasswordHash": "ABMzw8ytOgYmHvyGc4xkvMP/IzUSA5GvKqLBTeyjX06e56J9wauQaQiv5Rdhmc3rTQ==",
    "PasswordSalt": "chyK78xfQ1mOZtWFNN/Oug==",
    "RegistrationDate": new Date("2013-07-09"),
    "Following": [hrtisitoId, userId]
  },
  {
    "_id": hrtisitoId, //passowrd: alabala
    "Username": "hrisito",
    "Fullname": "Hristina Simeonova",
    "Email": "hristina.sim@gmail.com",
    "PasswordHash": "ABMzw8ytOgYmHvyGc4xkvMP/IzUSA5GvKqLBTeyjX06e56J9wauQaQiv5Rdhmc3rTQ==",
    "PasswordSalt": "chyK78xfQ1mOZtWFNN/Oug==",
    "RegistrationDate": new Date("2013-07-10"),
    "Following": [albsiId]
  },
  {
    "_id": userId, //passowrd: alabala
    "Username": "user",
    "Fullname": "Somebody",
    "Email": "kosta@yahoo.com",
    "PasswordHash": "ABMzw8ytOgYmHvyGc4xkvMP/IzUSA5GvKqLBTeyjX06e56J9wauQaQiv5Rdhmc3rTQ==",
    "PasswordSalt": "chyK78xfQ1mOZtWFNN/Oug==",
    "RegistrationDate": new Date("2013-10-09"),
    "Following": [albsiId, hrtisitoId]
  }
];

var comments = [];

for (var i = 0; i < 15; i++) {
  var retweetedBy = [],
      favoritedBy = [];

  if (i % 2 === 0) {
    favoritedBy.push(userId);
  }
  if (i % 3 === 0) {
    retweetedBy.push(hrtisitoId);
    favoritedBy.push(hrtisitoId);
  }
  if (i % 5 === 0) {
    retweetedBy.push(userId);
    favoritedBy.push(albsiId);
  }

  comments.push({
    'Author': albsiId,
    'Content': 'albsi comment' + i,
    'Replies': [
      {
        'Author': albsiId,
        'Content': 'reply1',
        'PublishDate': new Date()
      }, {
        'Author': hrtisitoId,
        'Content': 'reply2',
        'PublishDate': new Date()
      }, {
        'Author': userId,
        'Content': 'reply3',
        'PublishDate': new Date()
      }
    ],
    'PublishDate': new Date('2013-10-' + (i + 1)),
    'RetweetedBy': retweetedBy,
    'FavoritedBy': favoritedBy
  });
}

for (var i = 0; i < 15; i++) {
  var retweetedBy = [],
      favoritedBy = [];

  if (i % 2 === 0) {
    favoritedBy.push(userId);
  }
  if (i % 3 === 0) {
    retweetedBy.push(albsiId);
    favoritedBy.push(hrtisitoId);
  }
  if (i % 5 === 0) {
    retweetedBy.push(userId);
    favoritedBy.push(albsiId);
  }

  comments.push({
    'Author': hrtisitoId,
    'Content': 'hrisito comment' + i,
    'Replies': [
      {
        'Author': albsiId,
        'Content': 'reply1',
        'PublishDate': new Date()
      }, {
        'Author': hrtisitoId,
        'Content': 'reply2',
        'PublishDate': new Date()
      }, {
        'Author': userId,
        'Content': 'reply3',
        'PublishDate': new Date()
      }
    ],
    'PublishDate': new Date('2013-9-' + (i + 1)),
    'RetweetedBy': retweetedBy,
    'FavoritedBy': favoritedBy
  });
}

MongoClient.connect('mongodb://localhost:27017/hirundo', function (err, db) {
  if (!err) {
    console.log('We are connected');
  }
  var inserted = _.after(2, createIndexes),
      createdIndex = _.after(2, close)
      imageSaved = _.after(3, insertData);

  console.log('Saving images...');
  var bubblesGridStore = new GridStore(db, 'bubbles.jpg', 'w', { root: 'fs', content_type: 'image/jpeg' });
  bubblesGridStore.open(function (err, gridStore) {
    gridStore.writeFile('./images/powerpuff_girls_1.jpg', function (err, fileInfo) {
      gridStore.close(function (err, fileData) {
        users[0].ImgId = fileData._id;
        imageSaved();
      });
    });
  });

  var blossomGridStore = new GridStore(db, 'blossom.jpg', 'w', { root: 'fs', content_type: 'image/jpeg' });
  blossomGridStore.open(function (err, gridStore) {
    gridStore.writeFile('./images/powerpuff_girls_2.jpg', function (err, fileInfo) {
      gridStore.close(function (err, fileData) {
        users[1].ImgId = fileData._id;
        imageSaved();
      });
    });
  });

  var userGridStore = new GridStore(db, 'user.jpg', 'w', { root: 'fs', content_type: 'image/jpeg' });
  userGridStore.open(function (err, gridStore) {
    gridStore.writeFile('./images/user.jpg', function (err, fileInfo) {
      gridStore.close(function (err, fileData) {
        users[2].ImgId = fileData._id;
        imageSaved();
      });
    });
  });

  function insertData() {
    db.collection('user').drop(function (err, reply) {
      var user = db.collection('user');

      console.log('Inserting data into user collection.');
      user.insert(users, function (err, result) {
        inserted();
      });
    });

    db.collection('comment').drop(function (err, reply) {
      var comment = db.collection('comment');

      console.log('Inserting data into comment collection.');
      comment.insert(comments, function (err, result) {
        inserted();
      });
    });
  }

  function createIndexes() {
    db.collection('user').createIndex('Username', { unique: true, name: 'username_unique' }, function () {
      createdIndex();
    });

    db.collection('user').createIndex('Email', { unique: true, name: 'email_unique' }, function () {
      createdIndex();
    });
  }

  function close() {
    db.close();
  }
});