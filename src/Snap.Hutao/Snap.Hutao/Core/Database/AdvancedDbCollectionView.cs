﻿// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore;
using Snap.Hutao.Core.Database.Abstraction;
using Snap.Hutao.Model;
using Snap.Hutao.Model.Entity.Database;
using Snap.Hutao.UI.Xaml.Data;

namespace Snap.Hutao.Core.Database;

// The scope of the view follows the scope of the service provider.
internal sealed class AdvancedDbCollectionView<TEntity> : AdvancedCollectionView<TEntity>, IAdvancedDbCollectionView<TEntity>
    where TEntity : class, IAdvancedCollectionViewItem, ISelectable
{
    private readonly IServiceProvider serviceProvider;

    private bool savingToDatabase = true;

    public AdvancedDbCollectionView(IList<TEntity> source, IServiceProvider serviceProvider)
        : base(source, true)
    {
        this.serviceProvider = serviceProvider;
    }

    public IDisposable SuppressSavingToDatabase()
    {
        return new SavingToDatabaseSuppression(this);
    }

    protected override void OnCurrentChangedOverride()
    {
        if (serviceProvider is null || !savingToDatabase)
        {
            return;
        }

        TEntity? currentItem = CurrentItem;

        foreach (TEntity item in Source)
        {
            item.IsSelected = ReferenceEquals(item, currentItem);
        }

        using (IServiceScope scope = serviceProvider.CreateScope())
        {
            AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Set<TEntity>().ExecuteUpdate(update => update.SetProperty(entity => entity.IsSelected, false));

            if (currentItem is not null)
            {
                dbContext.Set<TEntity>().UpdateAndSave(currentItem);
            }
        }
    }

    private sealed class SavingToDatabaseSuppression : IDisposable
    {
        private readonly AdvancedDbCollectionView<TEntity> view;

        public SavingToDatabaseSuppression(AdvancedDbCollectionView<TEntity> view)
        {
            this.view = view;
            view.savingToDatabase = false;
        }

        public void Dispose()
        {
            view.savingToDatabase = true;
        }
    }
}

// The scope of the view follows the scope of the service provider.
[SuppressMessage("", "SA1402")]
internal sealed class AdvancedDbCollectionView<TEntityAccess, TEntity> : AdvancedCollectionView<TEntityAccess>, IAdvancedDbCollectionView<TEntityAccess>
    where TEntityAccess : class, IEntityAccess<TEntity>, IAdvancedCollectionViewItem
    where TEntity : class, ISelectable
{
    private readonly IServiceProvider serviceProvider;

    private bool savingToDatabase = true;

    public AdvancedDbCollectionView(IList<TEntityAccess> source, IServiceProvider serviceProvider)
        : base(source, true)
    {
        this.serviceProvider = serviceProvider;
    }

    public IDisposable SuppressSavingToDatabase()
    {
        return new SavingToDatabaseSuppression(this);
    }

    protected override void OnCurrentChangedOverride()
    {
        if (serviceProvider is null || !savingToDatabase)
        {
            return;
        }

        TEntityAccess? currentItem = CurrentItem;

        foreach (TEntityAccess item in Source)
        {
            item.Entity.IsSelected = ReferenceEquals(item, currentItem);
        }

        using (IServiceScope scope = serviceProvider.CreateScope())
        {
            AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Set<TEntity>().ExecuteUpdate(update => update.SetProperty(entity => entity.IsSelected, false));

            if (currentItem is not null)
            {
                dbContext.Set<TEntity>().UpdateAndSave(currentItem.Entity);
            }
        }
    }

    private sealed class SavingToDatabaseSuppression : IDisposable
    {
        private readonly AdvancedDbCollectionView<TEntityAccess, TEntity> view;

        public SavingToDatabaseSuppression(AdvancedDbCollectionView<TEntityAccess, TEntity> view)
        {
            this.view = view;
            view.savingToDatabase = false;
        }

        public void Dispose()
        {
            view.savingToDatabase = true;
        }
    }
}