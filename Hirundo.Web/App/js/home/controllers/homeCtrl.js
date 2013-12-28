/*global angular*/
(function (angular) {
  'use strict';

  function HomeCtrl($scope, $q, $window, $state, User, Comment) {
    var skip;

    $scope.hirundo = null;
    $scope.pending = false;

    $q.all({
      userData: User.userData.get().$promise,
      comments: User.userComments.query({ userId: $window.user.userId }).$promise
    }).then(function (res) {
      $scope.user = res.userData;
      $scope.comments = res.comments;

      skip = 20;
    });

    $scope.loadMore = function () {
      if (!$scope.pending) {
        $scope.pending = true;
        return User.userComments.query({ userId: $scope.user.userId, skip: skip })
            .$promise.then(function (comments) {
              $scope.comments = $scope.comments.concat(comments);
              skip += 20;
              $scope.pending = false;

              return comments.length !== 0;
            });
      }
    };

    $scope.sendHirundo = function () {
      var newComment = {
        'Author': $scope.user.userId,
        'Content': $scope.hirundo,
        'PublishDate': new Date()
      };

      return Comment.save(newComment).$promise.then(function () {
        $state.go('home');
      });
    };
  }

  HomeCtrl.$inject = ['$scope', '$q', '$window', '$state', 'User', 'Comment'];

  angular.module('home').controller('home.HomeCtrl', HomeCtrl);
}(angular));
