// Usage: <hd-users category="<title>" resource="<user_resource>" user="<id>"></hd-users>

/*global angular*/
(function (angular) {
  'use strict';

  function UsersDirective($navigation, $state, $window, User) {
    function UsersLink($scope, element, attrs) {
      var take = $window.config.itemsToTake,
          skip = take,
          userId = attrs.user,
          resource = attrs.resource,
          moreUsers = true;

      $navigation.loading = true;
      $scope.currentUser = $window.user.userId;

      User[resource].query({ userId: userId, take: take }).$promise.then(function (users) {
        $scope.users = users;
        $navigation.loading = false;
      });

      $scope.loadMore = function () {
        if (moreUsers && !$scope.pending && !$navigation.loading) {
          var promise = User[resource].query({ userId: userId, take: take, skip: skip }).$promise;

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

    return {
      restrict: 'E',
      replace: true,
      templateUrl: 'directives/users/usersDirective.html',
      scope: {
        category: '@'
      },
      link: UsersLink
    };
  }

  UsersDirective.$inject = ['navigation.NavigationConfig', '$state', '$window', 'User'];

  angular.module('directives').directive('hdUsers', UsersDirective);
}(angular));