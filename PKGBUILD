# PKGBUILD
pkgname=online-fix-importer
pkgver=1.0.6
pkgrel=1
pkgdesc="Onlinefix-launcher auto game installer"
arch=('x86_64')
license=('MIT')
depends=('icoextract')
makedepends=('dotnet-sdk' 'git' '7zip' 'icoextract' 'icoutils')
source=("$pkgname::git+https://github.com/kqnori/online-fix.git")
sha256sums=('SKIP')

build() {
    cd "$srcdir/$pkgname"
    dotnet publish onlinefix-out.csproj \
        --configuration Release \
        --runtime linux-x64 \
        --self-contained true \
        -p:PublishSingleFile=true \
        --output "$srcdir/publish"
}

options=('!strip')

package() {
    install -dm755 "$pkgdir/usr/lib/$pkgname"
    cp -a "$srcdir/publish/." "$pkgdir/usr/lib/$pkgname/"

    install -dm755 "$pkgdir/usr/bin"
    ln -s "/usr/lib/$pkgname/onlinefix-out" "$pkgdir/usr/bin/$pkgname"

    install -Dm644 "$srcdir/$pkgname/online-fix.desktop" \
        "$pkgdir/usr/share/kio/servicemenus/online-fix-importer.desktop"

    install -Dm644 "$srcdir/$pkgname/oflogo.png" \
        "$pkgdir/usr/share/icons/hicolor/48x48/apps/oflogo.png"
}
