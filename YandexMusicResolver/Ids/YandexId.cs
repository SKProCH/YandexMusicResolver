using System;

namespace YandexMusicResolver.Ids;

/// <summary>
/// Holds the yandex track id
/// </summary>
/// <remarks>
/// Since Yandex fucked up as usual, it will return an <see cref="long"/> ids for yandex's track, but will return <see cref="Guid"/> ids for user uploaded tracks
/// </remarks>
public struct YandexId {
    private long? _longId;
    private Guid? _guidId;

    /// <summary>
    /// Gets the <see cref="Guid"/> id
    /// </summary>
    /// <exception cref="InvalidOperationException">Current Id is not an Guid type. You can look at <see cref="IdType"/> to determine proper id type</exception>
    public Guid GuidId {
        get => _guidId ?? throw new InvalidOperationException($"Current Id is not an Guid type. Current type is {IdType}");
        set => _guidId = value;
    }

    /// <summary>
    /// Gets the <see cref="long"/> id
    /// </summary>
    /// <exception cref="InvalidOperationException">Current Id is not an long type. You can look at <see cref="IdType"/> to determine proper id type</exception>
    public long LongId {
        get => _longId ?? throw new InvalidOperationException($"Current Id is not an long type. Current type is {IdType}");
        set => _longId = value;
    }

    /// <summary>
    /// Gets the current id type
    /// </summary>
    public YandexIdType IdType { get; }

    /// <summary>
    /// Creates <see cref="YandexId"/> with <see cref="Guid"/> id
    /// </summary>
    /// <param name="id">The <see cref="Guid"/> id</param>
    public YandexId(Guid id) {
        IdType = YandexIdType.Guid;
        _guidId = id;
    }

    /// <summary>
    /// Creates <see cref="YandexId"/> with <see cref="long"/> id
    /// </summary>
    /// <param name="id">The <see cref="long"/> id</param>
    public YandexId(long id) {
        IdType = YandexIdType.Long;
        _longId = id;
    }

    /// <summary>
    /// Allows to implicitly cast <see cref="YandexId"/> to <see cref="Guid"/> 
    /// </summary>
    /// <param name="yandexId">Instance of <see cref="YandexId"/></param>
    /// <returns>The <see cref="Guid"/> value</returns>
    public static implicit operator Guid(YandexId yandexId) => yandexId.GuidId;
    
    /// <summary>
    /// Allows to implicitly cast <see cref="YandexId"/> to <see cref="long"/> 
    /// </summary>
    /// <param name="yandexId">Instance of <see cref="YandexId"/></param>
    /// <returns>The <see cref="long"/> value</returns>
    public static implicit operator long(YandexId yandexId) => yandexId.LongId;

    /// <summary>
    /// Allows to implicitly cast <see cref="long"/> to <see cref="YandexId"/>
    /// </summary>
    /// <param name="id"><see cref="long"/> value</param>
    /// <returns>The <see cref="YandexId"/> intance</returns>
    public static implicit operator YandexId(long id) => new(id);
    
    /// <summary>
    /// Allows to implicitly cast <see cref="Guid"/> to <see cref="YandexId"/>
    /// </summary>
    /// <param name="id"><see cref="Guid"/> value</param>
    /// <returns>The <see cref="YandexId"/> intance</returns>
    public static implicit operator YandexId(Guid id) => new(id);

    /// <inheritdoc />
    public override string ToString() {
        if (_longId is not null) {
            return _longId.ToString();
        }

        if (_guidId is not null) {
            return _guidId.ToString();
        }

        throw new NotSupportedException("Current Id type doesn't support to string conversion. Report this to the library author");
    }

    /// <summary>
    /// Tries to parse <see cref="YandexId"/> from the string
    /// </summary>
    /// <param name="s"><see cref="string"/> to parse the Id</param>
    /// <returns>Instance of <see cref="YandexId"/></returns>
    /// <exception cref="NotSupportedException">Current string cannot be converted to any known Id type. Check the string</exception>
    public static YandexId Parse(string s) {
        if (Guid.TryParse(s, out var guid)) {
            return new YandexId(guid);
        }

        if (long.TryParse(s, out var l)) {
            return new YandexId(l);
        }

        throw new NotSupportedException("Current string cannot be converted to any known Id type. Check the string");
    }

    /// <summary>
    /// Available types of Yandex ids
    /// </summary>
    public enum YandexIdType {
        /// <summary>
        /// Current id is long
        /// </summary>
        Long,
        /// <summary>
        /// Current id is Guid
        /// </summary>
        Guid
    }
}
