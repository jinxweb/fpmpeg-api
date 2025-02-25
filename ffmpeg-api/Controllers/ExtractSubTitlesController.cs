﻿using Ffmpeg.Library.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ffmpeg_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtractSubTitlesController : ControllerBase
    {
        ISubtitlesService _subtitlesService = null;

        public ExtractSubTitlesController(ISubtitlesService subtitlesService)
        {
                _subtitlesService = subtitlesService;
        }

        /// <summary>
        /// Extracts subtitles from a video file.  The video file must already contain subtitles
        /// and be less than 100MB in size.
        /// </summary>
        /// <param name="videoFile"></param>
        /// <param name="key"></param>
        /// <returns>Extracted Text</returns>
        [HttpGet]
        public async Task<IActionResult> Get(string videoFile, string key)
        {
            if (string.IsNullOrEmpty(videoFile))
            {
                return BadRequest("videoFile is required");
            }

            string accessKey = Environment.GetEnvironmentVariable("FFMPEG_API_KEY") ?? "1972";  

            if (string.IsNullOrEmpty(key) || key != accessKey)
            {
                return Unauthorized();
            }

            try
            {
                var subtitles = await _subtitlesService.ExtractSubTitles(videoFile);
                return Ok(subtitles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
