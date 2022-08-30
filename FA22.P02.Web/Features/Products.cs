﻿namespace FA22.P02.Web.Features
{
    public class Products
    {
        public class ProductDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal? Price { get; set; }
        }
    }
}
