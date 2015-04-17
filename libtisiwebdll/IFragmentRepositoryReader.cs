using System;

namespace mkcs.libtisiweb {

	public interface IFragmentRepositoryReader<T> {
		void ReadFragmentRepository(T repository, IFragmentRepository fragmentRepository);
	}
}

