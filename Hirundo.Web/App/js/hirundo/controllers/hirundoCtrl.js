/*global angular*/
(function (angular) {
  'use strict';

  function HirundoCtrl($scope, $state, $stateParams, $window, Comment) {
    var take = $window.config.itemsToTake,
        skip = take,
        commentId = $stateParams.id,
        moreReplies = true;

    $scope.loaded = false;
    $scope.currentUserId = $window.user.userId;
    $scope.currentUserImage = $window.user.userImg;
    $scope.comment = {};

    Comment.comment.get({ commentId: $stateParams.id }).$promise.then(function (comment) {
      $scope.comment.commentId = comment.commentData.commentId;
      $scope.comment.authorImg = comment.commentData.authorImg;
      $scope.comment.authorId = comment.commentData.authorId;
      $scope.comment.author = comment.commentData.author;
      $scope.comment.content = comment.commentData.content;
      $scope.comment.sharings = comment.commentDetails.sharings;
      $scope.comment.favorites = comment.commentDetails.favorites;
      $scope.comment.isShared = comment.commentData.isShared;
      $scope.comment.isFavorited = comment.commentData.isFavorited;
      $scope.comment.publishDate = comment.commentData.publishDate;
      $scope.comment.location = comment.commentData.location;
      $scope.comment.image = comment.commentData.image;
      $scope.comment.replies = comment.commentDetails.replies;

      $scope.loaded = true;
    });

    $scope.sendReply = function (hirundo) {
      var reply = {
        'Author': $window.user.userId,
        'Content': hirundo.content,
        'Location': hirundo.location,
        'Image': hirundo.image,
        'PublishDate': new Date()
      };

      return Comment.reply.save({ commentId: commentId }, reply).$promise
                          .then(function () {
        $state.go($state.$current, {}, { reload: true });
      });
    };

    $scope.loadMore = function () {
      if (moreReplies && !$scope.pending && $scope.loaded) {
        var promise = Comment.reply.query({ commentId: commentId, take: take, skip: skip })
                            .$promise;

        $scope.pending = true;
        promise.then(function (replies) {
          $scope.comment.replies = $scope.comment.replies.concat(replies);
          skip += take;
          $scope.pending = false;

          moreReplies = replies.length !== 0;
        });

        return promise;
      }
    };
  }

  HirundoCtrl.$inject = ['$scope', '$state', '$stateParams', '$window', 'Comment'];

  angular.module('hirundo').controller('hirundo.HirundoCtrl', HirundoCtrl);
}(angular));
