// Usage: <hd-comments model="<model_name>" category="<title>" load-more="<fn>"></hd-comments>

/*global angular*/
(function (angular) {
  'use strict';

  function CommentsDirective($window, User, Comment) {
    return {
      restrict: 'E',
      replace: true,
      templateUrl: 'directives/comments/commentsDirective.html',
      scope: {
        category: '@',
        model: '=',
        loadMore: '&',
        userImg: '='
      },
      link: function ($scope) {
        var skip = 20;

        $scope.pending = false;
        $scope.userId = $window.user.userId;

        $scope.loadMore = function () {
          if (!$scope.pending) {
            $scope.pending = true;
            return User.userComments.query({ userId: $scope.userId, skip: skip })
                .$promise.then(function (comments) {
              $scope.model = $scope.model.concat(comments);
              skip += 20;
              $scope.pending = false;

              return comments.length !== 0;
            });
          }
        };

        $scope.sendReply = function (comment) {
          var reply = {
            'Author': $window.user.userId,
            'Content': comment.newReply,
            'PublishDate': new Date()
          };

          return Comment.reply.save({ commentId: comment.commentId }, reply)
                        .$promise.then(function () {
            Comment.commentDetails.get({commentId: comment.commentId})
                   .$promise.then(function (commentDetails) {
              comment.replies = commentDetails.replies;
            });
          });
        };

        $scope.commentClick = function (comment) {
          comment.isExpanded = !comment.isExpanded;

          if (comment.isExpanded) {
            Comment.commentDetails.get({ commentId: comment.commentId })
                   .$promise.then(function (commentDetails) {
              comment.retweets = commentDetails.retweets;
              comment.favorites = commentDetails.favorites;
              comment.replies = commentDetails.replies;
            });
          }
        };
      }
    };
  }

  CommentsDirective.$inject = ['$window', 'User', 'Comment'];

  angular.module('directives').directive('hdComments', CommentsDirective);
}(angular));