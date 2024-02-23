﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Quiz.Core.Models;

namespace Quiz.Core.Interfaces;

public interface IRepository<T> 
    where T : class
{
    Task Add(T entity);
    Task Add(IEnumerable<T> entities);
    void Update(T entity);
    void Delete(T entity);
    void Delete(IEnumerable<T> entities);
    bool Exists(Expression<Func<T, bool>> whereCondition);
    IEnumerable<T> GetAll();
    IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes);
    IEnumerable<T> Get(Expression<Func<T, bool>> whereCondition);
    Task<T> GetById(params object[] keyValues);

}