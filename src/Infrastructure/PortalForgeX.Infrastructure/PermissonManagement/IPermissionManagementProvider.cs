namespace PortalForgeX.Infrastructure.PermissonManagement;

// TODO: implementation

///// <summary>
///// Interface for all AccessPoints (Entities) that needs permission to access features.
///// </summary>
///// <typeparam name="TKey"></typeparam>
//public interface IAccessPoint<TKey>
//{
//    /// <summary>
//    /// The reference Id tot he Access Point.
//    /// </summary>
//    TKey ProviderId { get; set; }

//    /// <summary>
//    /// Name of the permission/feature.
//    /// </summary>
//    string PermissionName { get; set; }

//    /// <summary>
//    /// Indicator if the AccessPoint has Access.
//    /// </summary>
//    bool HasAccess { get; set; }
//}

//public class UserAccess : IAccessPoint<Guid>
//{
//    [Column("UserId")]
//    public Guid ProviderId { get; set; }
//    public string PermissionName { get; set; } = null!;
//    public bool HasAccess { get; set; }
//    public Guid AuthorizedUserId { get; set; }
//    public DateTime? LastModificationTime { get; set; }
//}

///// <summary>
///// PermissionManagement interface for retrieving, granting and revoking permissions for AccessPoints.
///// </summary>
//public interface IPermissionManagementStore
//{
//    /// <summary>
//    /// Name of the Store for the AccessPoint (Entity)
//    /// </summary>
//    public string StoreName { get; }

//    /// <summary>
//    /// Stores will be ordered by PriorityLevel with 0 being the highest rank.
//    /// </summary>
//    public int PriorityLevel { get; }

//    /// <summary>
//    /// Get the AccessPoint.
//    /// </summary>
//    /// <typeparam name="TKey"></typeparam>
//    /// <param name="permission"></param>
//    /// <param name="targetId"></param>
//    /// <returns></returns>
//    Task<IAccessPoint<TKey>?> GetAsync<TKey>(string permission, TKey targetId);

//    /// <summary>
//    /// Grant permission to access.
//    /// </summary>
//    /// <typeparam name="TKey"></typeparam>
//    /// <param name="permission"></param>
//    /// <param name="providerId"></param>
//    /// <returns></returns>
//    Task<bool> GrantAccessAsync<TKey>(string permission, TKey targetId);

//    /// <summary>
//    /// Revoke permission to access.
//    /// </summary>
//    /// <typeparam name="TKey"></typeparam>
//    /// <param name="permission"></param>
//    /// <param name="providerId"></param>
//    /// <returns></returns>
//    Task<bool> RevokeAccessAsync<TKey>(string permission, TKey targetId);
//}

//public class UserPermissionManagementStore(
//    IDomainContext domainContext,
//    IFeatureManager featureManager
//    ) : IPermissionManagementStore
//{
//    public string StoreName => nameof(UserAccess);
//    public int PriorityLevel => 0;

//    private readonly ConcurrentBag<UserAccess> _access = [];
//    private readonly IDomainContext _domainContext = domainContext;
//    private readonly IFeatureManager _featureManager = featureManager;
//    private readonly DateTime? _lastUpdateTime;

//    private async Task LoadFromContext()
//    {
//        var enabledFeatureNames = new List<string>();
//        await foreach (var featureName in _featureManager.GetFeatureNamesAsync())
//        {
//            if (!await _featureManager.IsEnabledAsync(featureName))
//            {
//                continue;
//            }

//            enabledFeatureNames.Add(featureName);
//        }

//        if (enabledFeatureNames.Count == 0)
//        {
//            return;
//        }

//        // fill _access with _domainContext.UserAccess in featureNames
//        // _lastUpdateTime = _domainContext.UserAccess.Max(x => x.LastModificationTime);
//    }

//    public async Task<IAccessPoint<TKey>?> GetAsync<TKey>(string permission, TKey targetId)
//    {
//        if (_access.IsEmpty)
//        {
//            await LoadFromContext();
//        }
//        else
//        {
//            // if _lastUpdateTime != null && _domainContext.UserAccess.Max(x=>x.LastModificationTime) > _lastUpdateTime
//            // LoadFromContext
//        }

//        if (!Guid.TryParse(targetId?.ToString(), out var userId))
//        {
//            return null;
//        }

//        var accessPoint = _access.FirstOrDefault(x => x.ProviderId == userId && x.PermissionName == permission);

//        if (accessPoint is IAccessPoint<TKey> userAccess)
//        {
//            return userAccess;
//        }

//        return null;
//    }

//    public Task<bool> GrantAccessAsync<TKey>(string permission, TKey targetId)
//    {
//        throw new NotImplementedException();
//    }

//    public Task<bool> RevokeAccessAsync<TKey>(string permission, TKey targetId)
//    {
//        throw new NotImplementedException();
//    }
//}

//public interface IPermissionManager
//{
//    /// <summary>
//    /// Check if the User has access to the provided permission.
//    /// </summary>
//    /// <param name="userId"></param>
//    /// <param name="permission"></param>
//    /// <returns></returns>
//    Task<bool> HasAccessAsync(Guid userId, string permission);

//    /// <summary>
//    /// Grant access to the permission for the target.
//    /// </summary>
//    /// <typeparam name="TKey"></typeparam>
//    /// <param name="storeName"></param>
//    /// <param name="permission"></param>
//    /// <param name="providerId"></param>
//    /// <returns></returns>
//    Task<bool> GrantAccessAsync<TKey>(string storeName, string permission, TKey targetId);

//    /// <summary>
//    /// Revoke access to the permission for the target.
//    /// </summary>
//    /// <typeparam name="TKey"></typeparam>
//    /// <param name="storeName"></param>
//    /// <param name="permission"></param>
//    /// <param name="targetId"></param>
//    /// <returns></returns>
//    Task<bool> RevokeAccessAsync<TKey>(string storeName, string permission, TKey targetId);
//}

//public class PermissionManager : IPermissionManager
//{
//    public IEnumerable<IPermissionManagementStore> Stores { get; }

//    public PermissionManager(IEnumerable<IPermissionManagementStore> stores)
//    {
//        Stores = stores.OrderBy(x => x.PriorityLevel);
//    }

//    public async Task<bool> HasAccessAsync(Guid userId, string permission)
//    {
//        var hasAccess = false;
//        foreach (var provider in Stores)
//        {
//            var accessPoint = await provider.GetAsync(permission, userId);
//            if (accessPoint is null)
//            {
//                continue;
//            }

//            if (accessPoint.HasAccess)
//            {
//                hasAccess = true;
//                break;
//            }
//        }

//        return hasAccess;
//    }

//    public async Task<bool> GrantAccessAsync<TKey>(string storeName, string permission, TKey targetId)
//    {
//        var store = Stores.FirstOrDefault(x => x.StoreName == storeName);
//        return store is not null && await store.GrantAccessAsync(permission, targetId);
//    }

//    public async Task<bool> RevokeAccessAsync<TKey>(string storeName, string permission, TKey targetId)
//    {
//        var store = Stores.FirstOrDefault(x => x.StoreName == storeName);
//        return store is not null && await store.RevokeAccessAsync(permission, targetId);
//    }
//}
