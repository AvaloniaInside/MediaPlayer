namespace AvaloniaInside.MediaPlayer
{
    /// <summary>
    /// Represents a base class for media playback that can be disposed.
    /// </summary>
    public abstract class BasePlayback : IDisposable
    {
        private IMediaSource _mediaSource;

        /// <summary>
        /// Gets a flag indicating whether this <see cref="BasePlayback"/> is initialized.
        /// </summary>
        protected bool IsInitialized { get; private set; }

        /// <summary>
        /// Gets or sets the media source.
        /// An attempt to change it after initialization will result in an <see cref="InvalidOperationException"/>
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when tried to change the media source after initialization</exception>
        public IMediaSource? MediaSource
        {
            get => _mediaSource;
            set
            {
                if (IsInitialized)
                    throw new InvalidOperationException(
                        "Setting a new MediaSource after the Playback is initialized is not possible");

                _mediaSource = value;
            }
        }

        /// <summary>
        /// Disposes of this <see cref="BasePlayback"/>.
        /// </summary>
        public virtual void Dispose()
        {
        }

        /// <summary>
        /// Initializes this <see cref="BasePlayback"/>.
        /// </summary>
        public virtual void Initialize()
        {
            IsInitialized = true;
        }

        /// <summary>
        /// Updates the playback with the given time span.
        /// </summary>
        /// <param name="deltaTime">The time span to update the playback.</param>
        public abstract void Update(TimeSpan deltaTime);
    }
}