using System;

/*
 * *ti*ny *si*mple web management system
 * (C) Michael Kremser, 2003-2015
 * 
 * This is free software.
 * License: MIT
*/

namespace mkcs.libtisiweb {

	public interface IFragmentRepositoryReader<T> {
		void ReadFragmentRepository(T repository, IFragmentRepository fragmentRepository);
	}
}

