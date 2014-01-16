/*global angular*/
(function (angular) {
  'use strict';

  function SearchResultsCtrl($scope, $state, $stateParams, $window, User) {
    var take = $window.config.itemsToTake,
        skip = take,
        moreUsers = true;

    $scope.loaded = false;
    $scope.username = $stateParams.username;
    $scope.currentUser = $window.user.userId;

    User.userData.query({ username: $scope.username, take: take }).$promise.then(function (users) {
      $scope.users = users;
      $scope.loaded = true;
    });

    $scope.loadMore = function () {
      if (moreUsers && !$scope.pending && $scope.loaded) {
        var promise = User.userData.query({ username: $scope.username, take: take, skip: skip })
                                    .$promise;

        $scope.pending = true;
        promise.then(function (users) {
          $scope.users = $scope.users.concat(users);
          skip += take;
          $scope.pending = false;

          moreUsers = users.length !== 0;
        });

        return promise;
      }
    };

    $scope.follow = function (userId) {
      User.userFollowing.save({ userId: userId }).$promise.then(function () {
        $state.go($state.$current, {}, { reload: true });
      });
    };

    $scope.unfollow = function (userId) {
      User.userFollowing.remove({ userId: userId }).$promise.then(function () {
        $state.go($state.$current, {}, { reload: true });
      });
    };
  }

  SearchResultsCtrl.$inject = ['$scope', '$state', '$stateParams', '$window', 'User'];

  angular.module('navigation').controller('navigation.SearchResultsCtrl', SearchResultsCtrl);
}(angular));
