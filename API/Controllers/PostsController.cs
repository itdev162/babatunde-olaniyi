using System.Collections.Generic;
using Application.Posts;
using  System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Persistence;
using API;
using System;

namespace API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]

  public class PostsController : ControllerBase
  {

    private readonly DataContext context;

    /*
       private readonly IMediator mediator;

       public PostsController(IMediator mediator) => this.mediator = mediator;
  
       public async Task<ActionResult<List<Post>>> List()
       {
           return await this.mediator.Send(new List.Query());
       }
       */

       public PostsController(DataContext context)
       {
         this.context = context;
       }
       
       /// <summary>
       /// GET api/posts
       /// </summary>
       /// <returns>A list of posts</returns>
       [HttpGet]

       public ActionResult<List<Post>> Get()
       {
         return this.context.Posts.ToList();
       }

       /// <summary>
       /// GET api/post/[id]
       /// </summary>
       /// <param name="id">Post id</param>
       /// <returns>A single post</returns>

       [HttpGet("{id}")]

       public ActionResult<Post> GetById(Guid id)
       {
         return this.context.Posts.Find(id);
       }
       
       
       /// <summary>
       /// POST api/post
       /// </summary>

       ///<param name="request">JSON request containing post fields</param>
       ///<returns>A new post</returns>
       [HttpPost]

       public ActionResult<Post>Create([FromBody]Post request)
       {
         var post = new Post
         {
           Id = request.Id,
           Title = request.Title,
           Body = request.Body,
           Date = request.Date
         };

         context.Posts.Add(post);
         var success = context.SaveChanges() > 0;

         if (success)
         {
           return post;
         }

         throw new Exception("Error creating post");
       }

  
        /// <summary>
        /// PUT api/put
        /// </summary>
        /// <param name="request">JSON request containing one or more updated post fields</param>
        /// <returns>An updated post</returns>
       [HttpPut]

    public ActionResult<Post> Update([FromBody]Post request)
    {
      var post = context.Posts.Find(request.Id);

      if (post == null)
      {
        throw new Exception("Could not find post");
      }

      // Update the post properties with request values, if present.
      post.Title = request.Title != null ? request.Title : post.Title;
      post.Body = request.Body != null ? request.Body : post.Body;
      post.Date = request.Date != null ? request.Date : post.Date;

       var success = context.SaveChanges() > 0;

       if (success)
       {
         return post;
       }

       throw new Exception("Error updating post");
    }



    }
}