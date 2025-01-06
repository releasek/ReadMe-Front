using ReadMe_Front.Models.DTOs;
using ReadMe_Front.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReadMe_Front.Models.Repositories
{
    public class CategoryEFRepo
    {
        public List<CategoryDto> GetParent(string input)
        {
            using (var db = new AppDbContext())
            {
                var result = from product in db.Products
                             join category in db.Categories
                             on product.CategoryId equals category.Id
                             join parentCategory in db.ParentCategories
                             on category.ParentCategoryId equals parentCategory.Id
                             where parentCategory.ParentCategoriesName == input
                             select new CategoryDto
                             {
                                 Id = product.Id,
                                 Title = product.Title,
                                 Author = product.Author,
                                 Price = product.Price,
                                 CategoryName = category.CategoryName,
                                 ParentCategoriesName = input,
                                 ImageURL = product.ImageURL
                             };
                return result.ToList();
            }
        }

        public List<CategoryDto> GetSub(string input)
        {
            using (var db = new AppDbContext())
            {
                var result = from product in db.Products
                             join category in db.Categories
                             on product.CategoryId equals category.Id
                             join parentCategory in db.ParentCategories
                             on category.ParentCategoryId equals parentCategory.Id
                             where category.CategoryName == input
                             select new CategoryDto
                             {
                                 Id = product.Id,
                                 Title = product.Title,
                                 Author = product.Author,
                                 Price = product.Price,
                                 CategoryName = input,
                                 ParentCategoriesName = parentCategory.ParentCategoriesName,
                                 ImageURL = product.ImageURL
                             };
                return result.ToList();
            }
        }

        /// <summary>
        /// 篩選符合條件的商品
        /// </summary>
        /// <param name="input">篩選條件</param>
        /// <param name="pageNumber">第幾頁</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns></returns>
        public Paged<CategoryDto> Search(string input, int pageNumber, int pageSize)
        {

            //precondition check
            pageNumber = (pageNumber < 1) ? 1 : pageNumber;
            pageSize = (pageSize < 1) ? 12 : pageSize;

            using (var db = new AppDbContext())
            {
                var results = from product in db.Products
                              join category in db.Categories
                              on product.CategoryId equals category.Id
                              join parentCategory in db.ParentCategories
                              on category.ParentCategoryId equals parentCategory.Id
                              where (product.Title.Contains(input) ||
                                     product.Author.Contains(input) ||
                                     product.Publisher.Contains(input))
                              select new CategoryDto
                              {
                                  Id = product.Id,
                                  Title = product.Title,
                                  Author = product.Author,
                                  Publisher = product.Publisher,
                                  Price = product.Price,
                                  ImageURL = product.ImageURL,
                                  CategoryName = category.CategoryName,
                                  ParentCategoriesName = parentCategory.ParentCategoriesName,
                              };

                int recordCount = results.Count();

                List<CategoryDto> pagedData = results.OrderBy(x => x.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                return new Paged<CategoryDto>(pagedData, pageNumber, pageSize, recordCount);
            }
        }

    }

    /// <summary>
    /// 存放單頁紀錄及分頁資訊
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Paged<CategoryDto>
    {
        public List<CategoryDto> Data { get; set; }
        public PaginationInfo paginationInfo { get; set; }

        public Paged(List<CategoryDto> data, int pageNumber, int pageSize, int recordCount)
        {
            this.Data = data;
            this.paginationInfo = new PaginationInfo(pageNumber, pageSize, recordCount);
        }
    }


    /// <summary>
    /// 存放分頁資訊
    /// </summary>
    /// <param name="currentPage">頁碼</param>
    /// <param name="pageSize">每頁筆數</param>
    /// <param name="recordCount">總筆數</param>
    public class PaginationInfo
    {
        public PaginationInfo(int currentPage, int pageSize, int recordCount)
        {
            //precondition check
            currentPage = (currentPage < 1) ? 1 : currentPage;
            pageSize = (pageSize < 1) ? 12 : pageSize;
            recordCount = (recordCount < 0) ? 0 : recordCount;

            PageNumber = currentPage;
            PageSize = pageSize;
            RecordCount = recordCount;
        }

        public int PageNumber { get; }
        public int PageSize { get; }
        public int RecordCount { get; }



        /// <summary>
        /// 總頁數
        /// </summary>
        public int Pages
        {
            get
            {
                return (int)Math.Ceiling((double)RecordCount / PageSize);
            }
        }

        /// <summary>
        /// 是否顯示第一頁
        /// </summary>
        public bool ShowFirstPage
        {
            get
            {
                return PageNumber > 1;
            }
        }

        /// <summary>
        /// 是否顯示上一頁
        /// </summary>
        public bool ShowPrevPage
        {
            get
            {
                return PageNumber > 1;
            }
        }

        /// <summary>
        /// 是否顯示最末頁
        /// </summary>
        public bool ShowLastPage
        {
            get
            {
                return PageNumber < Pages;
            }
        }

        /// <summary>
        /// 是否顯示下一頁
        /// </summary>
        public bool ShowNextPage
        {
            get
            {
                return PageNumber < Pages;
            }
        }
    }
}